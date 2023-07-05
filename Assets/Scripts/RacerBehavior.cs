using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerBehavior : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float rightSpeed = 1f;
    [SerializeField] float upSpeed = 0.5f;
    [SerializeField] float minRightSpeed = 0.5f;
    [SerializeField] float maxRightSpeed = 4f;
    [SerializeField] float minUpSpeed = -2f;
    [SerializeField] float maxUpSpeed = 1.5f;
    [SerializeField] float minChangeTime = 0.3f;
    [SerializeField] float maxChangeTime = 4f;
    float timeSinceMoveUpdate = 0;
    float timeOfNextMoveUpdate;
    [SerializeField] float totalMoveModifier = 0.1f;
    
    enum PlayerStates {prerace, countdown, racing, finished};
    PlayerStates playerState = PlayerStates.prerace;
    bool isWinner = false;
    bool isDisabled = false;
    [SerializeField] float impactStunDuration = 0.5f;
    [SerializeField] float impactKnockbackPower = 2f;
    [SerializeField] float impactKnockdownChance = 0.5f;
    [SerializeField] float impactKnockdownDuration = 1f;

    //Updating functions
    void Start()
    {
        // Set the rb variable to this object's rigidbody
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (playerState == PlayerStates.racing && isDisabled == false){
            racingUpdate();
        }
    }

    void updateMovement(){
            // Set time to turn
            resetTimer();
            // Change speed
            changeRightSpeed();
            // Change vertical
            changeUpSpeed();
    }

    void racingUpdate(){
        // // Move the racer
        // moveRacer();
        // Update timer and change the angle if neccesary
        timeSinceMoveUpdate += Time.deltaTime;
        if (timeSinceMoveUpdate >= timeOfNextMoveUpdate){
            //updateMovement();
            updateMovementUseForce();

        }
    }

    //Behaviors
    void resetTimer(){
        // Set time to turn at a random number between 0.5 and 4
        timeSinceMoveUpdate = 0;
        timeOfNextMoveUpdate = Random.Range(minChangeTime, maxChangeTime);
    }
    void changeUpSpeed(){
        // Generate a new movement angle for the racer.
        float newUpSpeed = Random.Range(minUpSpeed, maxUpSpeed);
        upSpeed = newUpSpeed;
    }
    void changeRightSpeed(){
        // Change the move speed of the racer
        rightSpeed = Random.Range(minRightSpeed, maxRightSpeed);
    }
    void moveRacer(){
        // Move the racer based on the move vector
        rb.transform.Translate(rightSpeed * Time.deltaTime * totalMoveModifier, upSpeed * Time.deltaTime * totalMoveModifier, 0);
    }
    void changeSpeedForce(bool atMaxSpeed = false){
        if (atMaxSpeed){
            rightSpeed = maxRightSpeed;
            upSpeed = maxUpSpeed;
        }
        else {
            changeUpSpeed();
            changeRightSpeed();
        }

            // Change the move speed of the racer
            rb.velocity = Vector2.zero;
            
            rb.AddForce(new Vector2(rightSpeed, upSpeed) * totalMoveModifier, ForceMode2D.Impulse);


    }

    // Public functions
    public void StartRace(){
        playerState = PlayerStates.racing;
        //updateMovement();
        changeSpeedForce();
    }
    public void FinishRace(bool isWinner){
        playerState = PlayerStates.finished;
        rb.velocity = Vector2.zero;
        this.isWinner = isWinner;
    }

    // Collision functions
    private void OnCollisionEnter2D(Collision2D other) {
        // If the racer collidese with another racer, knock them both backwards
        if (other.gameObject.tag == "racer"){
            print("Racer collided with another racer");
            StartCoroutine(Knockback(other));
        }
    }
    IEnumerator Knockback(Collision2D other) {
        isDisabled = true;
        Vector2 direction = (gameObject.transform.position - other.gameObject.transform.position).normalized;
        rb.AddForce(direction * impactKnockbackPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(impactStunDuration);
        if (Random.Range(0f, 1f) <= impactKnockdownChance){
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(impactKnockdownDuration);
        }
        isDisabled = false;
        getUpAtMaxSpeed();

    }
    void getUpAtMaxSpeed() {
        timeSinceMoveUpdate = 0;
        timeOfNextMoveUpdate = maxChangeTime;
        changeSpeedForce(true);

    }

    void updateMovementUseForce(){
        // Use unity force instead of flat movement
        // Set time to turn
        resetTimer();
        // Change velocity
        changeSpeedForce();
    }
}
